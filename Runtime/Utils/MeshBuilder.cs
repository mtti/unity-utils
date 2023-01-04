/*
Copyright 2017-2020 Matti Hiltunen

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using mtti.Funcs.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace mtti.Funcs
{
    /// <summary>
    /// Helper class for generating meshes programmatically.
    /// </summary>
    public class MeshBuilder
    {
        private static List<Vector2Int> s_edges;

        private static HashSet<Vector2Int> s_edgeSet;

        private static List<int> s_newTriangles;

        private static Dictionary<Vector2Int, int> s_newVertices;

        public List<Vector3> Normals = new();

        public List<int> Triangles = new();

        public List<Vector2> UVs = new();

        public List<Vector3> Vertices = new();

        public int VertexCount
        {
            get { return Vertices.Count; }
        }

        public int TriangleCount
        {
            get { return Triangles.Count / 3; }
        }

        private static void AddEdge(
            int first,
            int second
        )
        {
            int smaller = first;
            int larger = second;

            if (first > second)
            {
                smaller = second;
                larger = first;
            }

            s_edgeSet.Add(new Vector2Int(smaller, larger));
        }

        private static void AddNewTriangle(
            int first,
            int second,
            int third
        )
        {
            s_newTriangles.Add(first);
            s_newTriangles.Add(second);
            s_newTriangles.Add(third);
        }

        /// <summary>
        /// Calculate vertex normals using a spherical projection from the mesh
        /// origin.
        /// </summary>
        public void CalculateSphericalNormals()
        {
            Normals.Clear();

            Vector3 direction;
            for (int i = 0, count = Vertices.Count; i < count; i++)
            {
                direction = Vertices[i].normalized;
                Normals.Add(direction);
            }
        }

        public void Subdivide(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                Subdivide();
            }
        }

        /// <summary>
        /// Split every triangle into three smaller ones while maintaining
        /// shared vertices.
        /// </summary>
        public void Subdivide()
        {
            FindEdges();

            // Initialize vertex index, which stores the new vertex of each edge
            if (s_newVertices == null)
            {
                s_newVertices = new Dictionary<Vector2Int, int>();
            }
            else
            {
                s_newVertices.Clear();
            }

            // Create a new vertex in the middle of each edge
            Vector3 vertex;
            foreach (Vector2Int edge in s_edgeSet)
            {
                vertex = (Vertices[edge.x] + Vertices[edge.y]) / 2.0f;
                Vertices.Add(vertex);
                s_newVertices[edge] = Vertices.Count - 1;
            }

            // Initialize the list of new triangles
            if (s_newTriangles == null)
            {
                s_newTriangles = new List<int>();
            }
            else
            {
                s_newTriangles.Clear();
            }

            if (s_edges == null)
            {
                s_edges = new List<Vector2Int>();
            }

            int old0;
            int old1;
            int old2;
            int new0;
            int new1;
            int new2;

            // Generate new triangles
            for (int ti = 0, count = Triangles.Count; ti < count; ti += 3)
            {
                old0 = Triangles[ti];
                old1 = Triangles[ti + 1];
                old2 = Triangles[ti + 2];

                // Get all edges of this triangle
                FindEdges(ti);
                s_edges.Clear();
                foreach (Vector2Int edge in s_edgeSet)
                {
                    s_edges.Add(edge);
                }

                new0 = s_newVertices[s_edges[0]];
                new1 = s_newVertices[s_edges[1]];
                new2 = s_newVertices[s_edges[2]];

                // Create the new sub-triangles
                AddNewTriangle(old0, new0, new1);
                AddNewTriangle(old1, new2, new0);
                AddNewTriangle(new0, new2, new1);
                AddNewTriangle(new1, new2, old2);
            }

            // Replace old triangle data with new
            Triangles.Clear();
            if (Triangles.Capacity < s_newTriangles.Count)
            {
                Triangles.Capacity = s_newTriangles.Count;
            }

            for (int i = 0, count = s_newTriangles.Count; i < count; i++)
            {
                Triangles.Add(s_newTriangles[i]);
            }
        }

        public void ApplySphericalProjection(float radius)
        {
            Vector3 direction;
            for (int i = 0, count = Vertices.Count; i < count; i++)
            {
                direction = Vertices[i].normalized;
                Vertices[i] = direction * radius;
            }
        }

        /// <summary>
        /// Clear all mesh data.
        /// </summary>
        public void Clear()
        {
            Vertices.Clear();
            Triangles.Clear();
            Normals.Clear();
        }

        /// <summary>
        /// Copy mesh data to a <see cref="UnityEngine.Mesh" />.
        /// </summary>
        [Obsolete("Use CopyTo instead")]
        public void Copy(Mesh target)
        {
            CopyTo(target);
        }

        /// <summary>
        /// Copy mesh data to a <see cref="UnityEngine.Mesh" />.
        /// </summary>
        public void CopyTo(Mesh target)
        {
            target.SetVertices(Vertices);
            target.SetTriangles(Triangles, 0);

            if (Normals.Count == Vertices.Count)
            {
                target.SetNormals(Normals);
            }

            if (UVs.Count == Vertices.Count)
            {
                target.SetUVs(0, UVs);
            }
        }

        /// <summary>
        /// Find vertex index pairs representing all the unique edges in the
        /// mesh.
        /// </summary>
        public int FindEdges(List<Vector2Int> result)
        {
            FindEdges();

            int count = 0;
            foreach (Vector2Int edge in s_edgeSet)
            {
                result.Add(edge);
                count++;
            }

            return count;
        }

        /// <summary>
        /// Find vertex index pairs representing all the edges of a triangle.
        /// </summary>
        public int FindEdges(
            int triangle,
            List<Vector2Int> result
        )
        {
            FindEdges(triangle);

            int count = 0;
            foreach (Vector2Int edge in s_edgeSet)
            {
                result.Add(edge);
                count++;
            }

            return count;
        }

        /// <summary>
        /// Find all connections between vertices, in both directions, indexed
        /// by source.
        /// </summary>
        public void FindVertexConnections(MultiValueDictionary<int, int> result)
        {
            FindEdges();
            foreach (Vector2Int edge in s_edgeSet)
            {
                result.Add(edge.x, edge.y);
                result.Add(edge.y, edge.x);
            }
        }

        private void ClearEdges()
        {
            if (s_edgeSet == null)
            {
                s_edgeSet = new HashSet<Vector2Int>();
            }
            else
            {
                s_edgeSet.Clear();
            }
        }

        /// <summary>
        /// Find the edges of a specific triangle.
        /// </summary>
        private void FindEdges(int triangle)
        {
            ClearEdges();
            AddEdge(Triangles[triangle], Triangles[triangle + 1]);
            AddEdge(Triangles[triangle], Triangles[triangle + 2]);
            AddEdge(Triangles[triangle + 1], Triangles[triangle + 2]);
        }

        /// <summary>
        /// Find mesh edges and place them into <c>s_edges</c> as pairs of
        /// vertex indexes.
        /// </summary>
        private void FindEdges()
        {
            ClearEdges();
            for (int ti = 0, count = Triangles.Count; ti < count; ti += 3)
            {
                AddEdge(Triangles[ti], Triangles[ti + 1]);
                AddEdge(Triangles[ti], Triangles[ti + 2]);
                AddEdge(Triangles[ti + 1], Triangles[ti + 2]);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using ISAAR.MSolve.LinearAlgebra.Commons;
using ISAAR.MSolve.LinearAlgebra.Vectors;

namespace ISAAR.MSolve.LinearAlgebra.Matrices
{
    /// <summary>
    /// Represents a diagonal (square) matrix.
    /// Authors: Serafeim Bakalakos
    /// </summary>
    public class DiagonalMatrix : IIndexable2D
    {
        private readonly double[] diagonal;
        private readonly Vector diagonalVector;

        private DiagonalMatrix(double[] diagonal)
        {
            this.diagonal = diagonal;
            this.diagonalVector = Vector.CreateFromArray(diagonal, false);
            this.NumColumns = this.NumRows = diagonal.Length;
        }

        /// <summary>
        /// See <see cref="IIndexable2D.NumColumns"/>.
        /// </summary>
        public int NumColumns { get; }

        /// <summary>
        /// See <see cref="IIndexable2D.NumRows"/>.
        /// </summary>
        public int NumRows { get; }

        /// <summary>
        /// See <see cref="IIndexable2D.this[int, int]"/>.
        /// </summary>
        public double this[int rowIdx, int colIdx]
        {
            //TODO: add index checking
            get { return (rowIdx == colIdx) ? diagonal[rowIdx] : 0.0; }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DiagonalMatrix"/> with <paramref name="diagonal"/> or a clone as its 
        /// internal array.
        /// </summary>
        /// <param name="diagonal">An array containing the diagonal entries of the matrix.</param>
        /// <param name="copyArray">
        /// If true, <paramref name="diagonal"/> will be copied and the new <see cref="DiagonalMatrix"/> instance will have a 
        /// reference to the copy, which is safer. If false, the new matrix will have a reference to <paramref name="diagonal"/> 
        /// itself, which is faster.
        /// </param>
        public static DiagonalMatrix CreateFromArray(double[] diagonal, bool copyArray = false)
        {
            if (copyArray)
            {
                var clone = new double[diagonal.Length];
                Array.Copy(diagonal, clone, clone.Length);
                return new DiagonalMatrix(clone);
            }
            else return new DiagonalMatrix(diagonal);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DiagonalMatrix"/> with its diagonal entries being 1.0. Non-diagonal entries
        /// are 0.0 as always.
        /// </summary>
        /// <param name="order">The number of rows/columns of the matrix.</param>
        public static DiagonalMatrix CreateIdentity(int order)
        {
            var diagonal = new double[order];
            for (int i = 0; i < order; ++i) diagonal[i] = 1.0;
            return new DiagonalMatrix(diagonal);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DiagonalMatrix"/> with its diagonal entries being <paramref name="value"/>. 
        /// Non-diagonal entries are 0.0 as always.
        /// </summary>
        /// <param name="order">The number of rows/columns of the matrix.</param>
        /// <param name="value">The value that all diagonal entries will be set equal to.</param>
        public static DiagonalMatrix CreateWithValue(int order, double value)
        {
            var diagonal = new double[order];
            for (int i = 0; i < order; ++i) diagonal[i] = value;
            return new DiagonalMatrix(diagonal);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DiagonalMatrix"/> with its diagonal (and non-diagonal) entries being 0.0.
        /// </summary>
        /// <param name="order">The number of rows/columns of the matrix.</param>
        public static DiagonalMatrix CreateZero(int order) => new DiagonalMatrix(new double[order]);

        /// <summary>
        /// Multiplies this matrix with a vector.
        /// </summary>
        /// <param name="vector">
        /// The vector that will be multiplied. Constraints: 
        /// <paramref name="vector"/>.<see cref="IIndexable1D.Length"/> == this.<see cref="IIndexable2D.NumColumns"/>.
        /// </param>
        /// <exception cref="Exceptions.NonMatchingDimensionsException">
        /// Thrown if <paramref name="vector"/> violates the described constraint.
        /// </exception>
        public Vector Multiply(Vector vector)
        {
            Preconditions.CheckMultiplicationDimensions(this.NumColumns, vector.Length);
            return diagonalVector.MultiplyEntrywise(vector);
        }

        /// <summary>
        /// See <see cref="IIndexable2D.Equals(IIndexable2D, double)"/>.
        /// </summary>
        public bool Equals(IIndexable2D other, double tolerance = 1E-13) => DenseStrategies.AreEqual(this, other, tolerance);
    }
}

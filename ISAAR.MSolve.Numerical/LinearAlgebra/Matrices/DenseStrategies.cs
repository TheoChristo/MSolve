﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISAAR.MSolve.Numerical.LinearAlgebra.Logging;
using ISAAR.MSolve.Numerical.LinearAlgebra.Testing.Utilities;

namespace ISAAR.MSolve.Numerical.LinearAlgebra.Matrices
{
    internal static class DenseStrategies
    {
        internal static bool AreEqual(IIndexable2D matrix1, IIndexable2D matrix2, double tolerance = 1e-13)
        {
            if ((matrix1.NumRows != matrix2.NumRows) || (matrix1.NumColumns != matrix2.NumColumns)) return false;
            var comparer = new ValueComparer(1e-13);
            for (int j = 0; j < matrix1.NumColumns; ++j)
            {
                for (int i = 0; i < matrix1.NumRows; ++i)
                {
                    if (!comparer.AreEqual(matrix1[i, j], matrix2[i, j])) return false;
                }
            }
            return true;
        }

        internal static Matrix DoEntrywise(IIndexable2D matrix1, IIndexable2D matrix2, 
            Func<double, double, double> binaryOperation)
        {
            Preconditions.CheckSameMatrixDimensions(matrix1, matrix2);
            var result = Matrix.CreateZero(matrix1.NumRows, matrix1.NumColumns);
            for (int j = 0; j < matrix1.NumColumns; ++j)
            {
                for (int i = 0; i < matrix1.NumRows; ++i)
                {
                    result[i, j] = binaryOperation(matrix1[i, j], matrix2[i, j]);
                }
            }
            return result;
        }

        internal static void WriteToConsole(IIndexable2D matrix, Array2DFormatting format = null)
        {
            if (format == null) format = Array2DFormatting.Brackets;
            Console.Write(format.ArrayStart);

            // First row
            Console.Write(format.RowSeparator + format.RowStart);
            Console.Write(matrix[0, 0]);
            for (int j = 1; j < matrix.NumColumns; ++j)
            {
                Console.Write(format.ColSeparator + matrix[0, j]);
            }
            Console.Write(format.RowEnd);

            // Subsequent rows
            for (int i = 1; i < matrix.NumRows; ++i)
            {
                Console.Write(format.RowSeparator + format.RowStart);
                Console.Write(matrix[i, 0]);
                for (int j = 1; j < matrix.NumColumns; ++j)
                {
                    Console.Write(format.ColSeparator + matrix[i, j]);
                }
                Console.Write(format.RowEnd);
            }
            Console.Write(format.RowSeparator + format.ArrayEnd);
        }

        internal static Matrix Transpose(ITransposable matrix)
        {
            var result = Matrix.CreateZero(matrix.NumColumns, matrix.NumRows);
            for (int j = 0; j < matrix.NumColumns; ++j)
            {
                for (int i = 0; i < matrix.NumRows; ++i)
                {
                    result[j, i] = matrix[i, j];
                }
            }
            return result;
        }

        internal static void WriteToFile(IIndexable2D matrix, string path, bool append = false, Array2DFormatting format = null)
        {
            if (format == null) format = Array2DFormatting.Brackets;
            //TODO: incorporate this and WriteToConsole into a common function, where the user passes the stream and an object to 
            //deal with formating. Also add support for relative paths. Actually these methods belong in the "Logging" project, 
            // but since they are extremely useful they are implemented here for now.
            using (var writer = new StreamWriter(path, append))
            {
                writer.Write(format.ArrayStart);

                // First row
                writer.Write(format.RowSeparator + format.RowStart);
                writer.Write(matrix[0, 0]);
                for (int j = 1; j < matrix.NumColumns; ++j)
                {
                    writer.Write(format.ColSeparator + matrix[0, j]);
                }
                writer.Write(format.RowEnd);

                // Subsequent rows
                for (int i = 1; i < matrix.NumRows; ++i)
                {
                    writer.Write(format.RowSeparator + format.RowStart);
                    writer.Write(matrix[i, 0]);
                    for (int j = 1; j < matrix.NumColumns; ++j)
                    {
                        writer.Write(format.ColSeparator + matrix[i, j]);
                    }
                    writer.Write(format.RowEnd);
                }
                writer.Write(format.RowSeparator + format.ArrayEnd);

#if DEBUG
                writer.Flush(); // If the user inspects the file while debugging, make sure the contentss are written.
#endif
            }
        }
    }
}

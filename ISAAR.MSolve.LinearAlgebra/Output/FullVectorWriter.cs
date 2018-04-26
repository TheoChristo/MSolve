﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISAAR.MSolve.LinearAlgebra.Vectors;

namespace ISAAR.MSolve.LinearAlgebra.Output
{
    public class FullVectorWriter: MatrixWriter
    {
        private readonly Array1DFormatting format;
        private readonly bool writeLengthFirst = false;
        private readonly IVectorView vector;
        

        public FullVectorWriter(IVectorView vector, bool writeLengthFirst = false, Array1DFormatting format = null)
        {
            this.format = (format == null) ? Array1DFormatting.Plain : format;
            this.writeLengthFirst = writeLengthFirst;
            this.vector = vector;
        }

        public static INumericFormat NumericFormat { get; set; } = new ExponentialFormat { NumDecimalDigits = 6 };

        protected override void WriteToStream(StreamWriter writer)
        {
            string numberFormat = NumericFormat.GetRealNumberFormat();
            string separator = format.Separator;
            if (writeLengthFirst) writer.Write(vector.Length + " ");
            writer.Write(format.Start);
            writer.Write(string.Format(numberFormat, vector[0]));
            for (int i = 1; i < vector.Length; ++i)
            {
                writer.Write(separator + string.Format(numberFormat, vector[i]));
            }
            writer.WriteLine(format.End);
        }
    }
}

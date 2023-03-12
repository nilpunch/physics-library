using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public struct Matrix3x3
    {
        public SoftFloat I00;
        public SoftFloat I01;
        public SoftFloat I02;
        public SoftFloat I10;
        public SoftFloat I11;
        public SoftFloat I12;
        public SoftFloat I20;
        public SoftFloat I21;
        public SoftFloat I22;

        public SoftVector3 Row0
        {
            get { return new SoftVector3(I00, I01, I02); }
            set
            {
                I00 = value.X;
                I01 = value.Y;
                I02 = value.Z;
            }
        }

        public SoftVector3 Row1
        {
            get { return new SoftVector3(I10, I11, I12); }
            set
            {
                I10 = value.X;
                I11 = value.Y;
                I12 = value.Z;
            }
        }

        public SoftVector3 Row2
        {
            get { return new SoftVector3(I20, I21, I22); }
            set
            {
                I20 = value.X;
                I21 = value.Y;
                I22 = value.Z;
            }
        }

        public SoftVector3 Col0
        {
            get { return new SoftVector3(I00, I10, I20); }
            set
            {
                I00 = value.X;
                I10 = value.Y;
                I20 = value.Z;
            }
        }

        public SoftVector3 Col1
        {
            get { return new SoftVector3(I01, I11, I21); }
            set
            {
                I01 = value.X;
                I11 = value.Y;
                I21 = value.Z;
            }
        }

        public SoftVector3 Col2
        {
            get { return new SoftVector3(I02, I12, I22); }
            set
            {
                I02 = value.X;
                I12 = value.Y;
                I22 = value.Z;
            }
        }

        public static Matrix3x3 FromRows(SoftVector3 row0, SoftVector3 row1, SoftVector3 row2)
        {
            return new Matrix3x3(row0.X, row0.Y, row0.Z, row1.X, row1.Y, row1.Z, row2.X, row2.Y, row2.Z);
        }

        public static Matrix3x3 FromCols(SoftVector3 col0, SoftVector3 col1, SoftVector3 col2)
        {
            return new Matrix3x3(col0.X, col1.X, col2.X, col0.Y, col1.Y, col2.Y, col0.Z, col1.Z, col2.Z);
        }

        public Matrix3x3(SoftFloat i00, SoftFloat i01, SoftFloat i02, SoftFloat i10, SoftFloat i11, SoftFloat i12, SoftFloat i20, SoftFloat i21,
            SoftFloat i22)
        {
            I00 = i00;
            I01 = i01;
            I02 = i02;
            I10 = i10;
            I11 = i11;
            I12 = i12;
            I20 = i20;
            I21 = i21;
            I22 = i22;
        }

        public Matrix3x3 Inverted
        {
            get { return Inverse(this); }
        }

        public Matrix3x3 Transposed
        {
            get { return Transpose(this); }
        }

        private static readonly Matrix3x3 kIdentity =
            new Matrix3x3(SoftFloat.One, SoftFloat.Zero, SoftFloat.Zero, SoftFloat.Zero, SoftFloat.One, SoftFloat.Zero, SoftFloat.Zero, SoftFloat.Zero, SoftFloat.One);

        public static Matrix3x3 Identity
        {
            get { return kIdentity; }
        }

        // Cross(A, B) = Skew(A) B = Skew(-B) A = Cross(-B, A)
        public static Matrix3x3 Skew(SoftVector3 v)
        {
            return new Matrix3x3(SoftFloat.Zero, -v.Z, v.Y, v.Z, SoftFloat.Zero, -v.X, -v.Y, v.X, SoftFloat.Zero);
        }

        public static Matrix3x3 operator +(Matrix3x3 a, Matrix3x3 b)
        {
            return FromRows(a.Row0 + b.Row0, a.Row1 + b.Row1, a.Row2 + b.Row2);
        }

        public static Matrix3x3 operator -(Matrix3x3 a, Matrix3x3 b)
        {
            return FromRows(a.Row0 - b.Row0, a.Row1 - b.Row1, a.Row2 - b.Row2);
        }

        public static Matrix3x3 operator *(SoftFloat s, Matrix3x3 m)
        {
            return FromRows(s * m.Row0, s * m.Row1, s * m.Row2);
        }

        public static Matrix3x3 operator *(Matrix3x3 m, SoftFloat s)
        {
            return s * m;
        }

        public static SoftVector3 operator *(Matrix3x3 m, SoftVector3 v)
        {
            return new SoftVector3(SoftVector3.Dot(m.Row0, v), SoftVector3.Dot(m.Row1, v), SoftVector3.Dot(m.Row2, v));
        }

        public static SoftVector3 operator *(SoftVector3 v, Matrix3x3 m)
        {
            return new SoftVector3(SoftVector3.Dot(v, m.Col0), SoftVector3.Dot(v, m.Col1), SoftVector3.Dot(v, m.Col2));
        }

        public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
        {
            return FromCols(a * b.Col0, a * b.Col1, a * b.Col2);
        }

        public static SoftFloat Mul(SoftVector3 a, Matrix3x3 m, SoftVector3 b)
        {
            return SoftVector3.Dot(a * m, b);
        }

        public static Matrix3x3 Inverse(Matrix3x3 m)
        {
            // too lazy to optimize
            // help, compiler
            SoftFloat det = m.I00 * m.I11 * m.I22 + m.I01 * m.I12 * m.I20 + m.I10 * m.I21 * m.I02 - m.I02 * m.I11 * m.I20 -
                            m.I01 * m.I10 * m.I22 - m.I12 * m.I21 * m.I00;

            // I trust that this inertia tensor is well-constructed
            SoftFloat detInv = SoftFloat.One / det;

            return new Matrix3x3((m.I11 * m.I22 - m.I21 * m.I12) * detInv, (m.I12 * m.I20 - m.I10 * m.I22) * detInv,
                (m.I10 * m.I21 - m.I20 * m.I11) * detInv, (m.I02 * m.I21 - m.I01 * m.I22) * detInv,
                (m.I00 * m.I22 - m.I02 * m.I20) * detInv, (m.I20 * m.I01 - m.I00 * m.I21) * detInv,
                (m.I01 * m.I12 - m.I02 * m.I11) * detInv, (m.I10 * m.I02 - m.I00 * m.I12) * detInv,
                (m.I00 * m.I11 - m.I10 * m.I01) * detInv);
        }

        public static Matrix3x3 Transpose(Matrix3x3 m)
        {
            return new Matrix3x3(m.I00, m.I10, m.I20, m.I01, m.I11, m.I21, m.I02, m.I12, m.I22);
        }
    }
}

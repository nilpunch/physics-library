using PluggableMath;

namespace GameLibrary.Physics
{
    public struct Matrix3x3<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public Operand<TNumber> I00;
        public Operand<TNumber> I01;
        public Operand<TNumber> I02;
        public Operand<TNumber> I10;
        public Operand<TNumber> I11;
        public Operand<TNumber> I12;
        public Operand<TNumber> I20;
        public Operand<TNumber> I21;
        public Operand<TNumber> I22;

        public Vector3<TNumber> Row0
        {
            get { return new Vector3<TNumber>(I00, I01, I02); }
            set
            {
                I00 = value.X;
                I01 = value.Y;
                I02 = value.Z;
            }
        }

        public Vector3<TNumber> Row1
        {
            get { return new Vector3<TNumber>(I10, I11, I12); }
            set
            {
                I10 = value.X;
                I11 = value.Y;
                I12 = value.Z;
            }
        }

        public Vector3<TNumber> Row2
        {
            get { return new Vector3<TNumber>(I20, I21, I22); }
            set
            {
                I20 = value.X;
                I21 = value.Y;
                I22 = value.Z;
            }
        }

        public Vector3<TNumber> Col0
        {
            get { return new Vector3<TNumber>(I00, I10, I20); }
            set
            {
                I00 = value.X;
                I10 = value.Y;
                I20 = value.Z;
            }
        }

        public Vector3<TNumber> Col1
        {
            get { return new Vector3<TNumber>(I01, I11, I21); }
            set
            {
                I01 = value.X;
                I11 = value.Y;
                I21 = value.Z;
            }
        }

        public Vector3<TNumber> Col2
        {
            get { return new Vector3<TNumber>(I02, I12, I22); }
            set
            {
                I02 = value.X;
                I12 = value.Y;
                I22 = value.Z;
            }
        }

        public static Matrix3x3<TNumber> FromRows(Vector3<TNumber> row0, Vector3<TNumber> row1, Vector3<TNumber> row2)
        {
            return new Matrix3x3<TNumber>(row0.X, row0.Y, row0.Z, row1.X, row1.Y, row1.Z, row2.X, row2.Y, row2.Z);
        }

        public static Matrix3x3<TNumber> FromCols(Vector3<TNumber> col0, Vector3<TNumber> col1, Vector3<TNumber> col2)
        {
            return new Matrix3x3<TNumber>(col0.X, col1.X, col2.X, col0.Y, col1.Y, col2.Y, col0.Z, col1.Z, col2.Z);
        }

        public Matrix3x3(Operand<TNumber> i00, Operand<TNumber> i01, Operand<TNumber> i02, Operand<TNumber> i10, Operand<TNumber> i11, Operand<TNumber> i12, Operand<TNumber> i20, Operand<TNumber> i21,
            Operand<TNumber> i22)
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

        public Matrix3x3<TNumber> Inverted
        {
            get { return Inverse(this); }
        }

        public Matrix3x3<TNumber> Transposed
        {
            get { return Transpose(this); }
        }

        private static readonly Matrix3x3<TNumber> kIdentity =
            new Matrix3x3<TNumber>(Operand<TNumber>.One, Operand<TNumber>.Zero, Operand<TNumber>.Zero, Operand<TNumber>.Zero, Operand<TNumber>.One, Operand<TNumber>.Zero, Operand<TNumber>.Zero, Operand<TNumber>.Zero, Operand<TNumber>.One);

        public static Matrix3x3<TNumber> Identity
        {
            get { return kIdentity; }
        }

        // Cross(A, B) = Skew(A) B = Skew(-B) A = Cross(-B, A)
        public static Matrix3x3<TNumber> Skew(Vector3<TNumber> v)
        {
            return new Matrix3x3<TNumber>(Operand<TNumber>.Zero, -v.Z, v.Y, v.Z, Operand<TNumber>.Zero, -v.X, -v.Y, v.X, Operand<TNumber>.Zero);
        }

        public static Matrix3x3<TNumber> operator +(Matrix3x3<TNumber> a, Matrix3x3<TNumber> b)
        {
            return FromRows(a.Row0 + b.Row0, a.Row1 + b.Row1, a.Row2 + b.Row2);
        }

        public static Matrix3x3<TNumber> operator -(Matrix3x3<TNumber> a, Matrix3x3<TNumber> b)
        {
            return FromRows(a.Row0 - b.Row0, a.Row1 - b.Row1, a.Row2 - b.Row2);
        }

        public static Matrix3x3<TNumber> operator *(Operand<TNumber> s, Matrix3x3<TNumber> m)
        {
            return FromRows(s * m.Row0, s * m.Row1, s * m.Row2);
        }

        public static Matrix3x3<TNumber> operator *(Matrix3x3<TNumber> m, Operand<TNumber> s)
        {
            return s * m;
        }

        public static Vector3<TNumber> operator *(Matrix3x3<TNumber> m, Vector3<TNumber> v)
        {
            return new Vector3<TNumber>(Vector3<TNumber>.Dot(m.Row0, v), Vector3<TNumber>.Dot(m.Row1, v), Vector3<TNumber>.Dot(m.Row2, v));
        }

        public static Vector3<TNumber> operator *(Vector3<TNumber> v, Matrix3x3<TNumber> m)
        {
            return new Vector3<TNumber>(Vector3<TNumber>.Dot(v, m.Col0), Vector3<TNumber>.Dot(v, m.Col1), Vector3<TNumber>.Dot(v, m.Col2));
        }

        public static Matrix3x3<TNumber> operator *(Matrix3x3<TNumber> a, Matrix3x3<TNumber> b)
        {
            return FromCols(a * b.Col0, a * b.Col1, a * b.Col2);
        }

        public static Operand<TNumber> Mul(Vector3<TNumber> a, Matrix3x3<TNumber> m, Vector3<TNumber> b)
        {
            return Vector3<TNumber>.Dot(a * m, b);
        }

        public static Matrix3x3<TNumber> Inverse(Matrix3x3<TNumber> m)
        {
            // too lazy to optimize
            // help, compiler
            Operand<TNumber> det = m.I00 * m.I11 * m.I22 + m.I01 * m.I12 * m.I20 + m.I10 * m.I21 * m.I02 - m.I02 * m.I11 * m.I20 -
                            m.I01 * m.I10 * m.I22 - m.I12 * m.I21 * m.I00;

            // I trust that this inertia tensor is well-constructed
            Operand<TNumber> detInv = Operand<TNumber>.One / det;

            return new Matrix3x3<TNumber>((m.I11 * m.I22 - m.I21 * m.I12) * detInv, (m.I12 * m.I20 - m.I10 * m.I22) * detInv,
                (m.I10 * m.I21 - m.I20 * m.I11) * detInv, (m.I02 * m.I21 - m.I01 * m.I22) * detInv,
                (m.I00 * m.I22 - m.I02 * m.I20) * detInv, (m.I20 * m.I01 - m.I00 * m.I21) * detInv,
                (m.I01 * m.I12 - m.I02 * m.I11) * detInv, (m.I10 * m.I02 - m.I00 * m.I12) * detInv,
                (m.I00 * m.I11 - m.I10 * m.I01) * detInv);
        }

        public static Matrix3x3<TNumber> Transpose(Matrix3x3<TNumber> m)
        {
            return new Matrix3x3<TNumber>(m.I00, m.I10, m.I20, m.I01, m.I11, m.I21, m.I02, m.I12, m.I22);
        }
    }
}

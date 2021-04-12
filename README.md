# FxNet

FxNet is a simple fixed-point math library written in C# for .NET Standard 2.0.

It is developed just for fun as a pet-project.

### Features

- Basic math functions (Sin, Cos, Asin, Atan2, Sqrt)  based on compact LUT
- Algebraic types for vectors, matrices and quaternions
- Simple scene graph (only for rigid transformations)
- Collision checking with GJK (2D only)
- PCG random generator

### Details

- Fixed point number is 64-bit wide, 18 bits is used for fractional part
- Matrices are col-major
- Left-handed coordinate system: X - Right, Y - Up, Z - Forward (directed away from the viewer)
- Rotation order: `ZXY = Qy * (Qx * Qz)` (similar to Unity)

### TODO

- Implement `Slerp` for `FxQuat`
- Better IL2CPP support (use special attributes)
- Implement GJK for 3D-collisions

#pragma once
#include "mkl.h"

extern "C"  _declspec(dllexport)
void Interpolation(MKL_INT nx, MKL_INT ny, double* x, double* y,
    double* bc, double* scoeff, MKL_INT nsite, double* site,
    MKL_INT ndorder, MKL_INT * dorder, double* interpolationValues,
    MKL_INT nlim, double* llim, double* rlim, double* integrationValues, int& ret);
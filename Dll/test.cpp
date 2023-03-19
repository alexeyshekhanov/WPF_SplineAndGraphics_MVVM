#include "pch.h"
#include "mkl.h"

extern "C"  _declspec(dllexport)
void Interpolation(MKL_INT nx, MKL_INT ny, double* x, double* y,
    double* bc, double* scoeff, MKL_INT nsite, double* site,
    MKL_INT ndorder, MKL_INT * dorder, double* interpolationValues,
    MKL_INT nlim, double* llim, double* rlim, double* integrationValues, int& err)
{
    try
    {
        int res;
        DFTaskPtr task;
        res = dfdNewTask1D(&task, nx, x, DF_NON_UNIFORM_PARTITION, ny, y, DF_MATRIX_STORAGE_ROWS);
        if (res != DF_STATUS_OK)
        {
            err = -1;
        }
        res = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL,
            DF_BC_2ND_LEFT_DER | DF_BC_2ND_RIGHT_DER, bc, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
        if (res != DF_STATUS_OK)
        {
            err = -2;
        }
        res = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
        if (res != DF_STATUS_OK)
        {
            err = -3;
        }
        res = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, nsite, site,
            DF_NON_UNIFORM_PARTITION, ndorder, dorder, NULL, interpolationValues,
            DF_MATRIX_STORAGE_ROWS, NULL);
        if (res != DF_STATUS_OK)
        {
            err = -4;
        }
        res = dfdIntegrate1D(task, DF_METHOD_PP, nlim, llim, DF_NON_UNIFORM_PARTITION, rlim,
            DF_UNIFORM_PARTITION, NULL, NULL, integrationValues, DF_MATRIX_STORAGE_ROWS);
        if (res != DF_STATUS_OK)
        {
            err = -5;
        }
        res = dfDeleteTask(&task);
        if (res != DF_STATUS_OK)
        {
            err = -6;
        }

        err = 0;
    }
    catch (...)
    {
        err = -7;
    }
}
#pragma once
#include "render.h"

geometry makePlane(float width, float height, int rows, int cols, bool randomizeColors);

geometry loadObj(const char * fileName);

#ifndef CITY_H
#define CITY_H

#include "MapObject.h"

class City :
    public MapObject
{
public:
    City(GLfloat x, GLfloat y, GLfloat z, GLfloat width, GLfloat height, char* path);
};

#endif
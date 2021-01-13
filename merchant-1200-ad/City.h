#ifndef CITY_H
#define CITY_H

#include "MapObject.h"
#include "Scene.h"

class City :
    public MapObject
{
public:

    City(GLfloat x, GLfloat y, GLfloat z, GLfloat width, GLfloat height, char* path, std::string name);

};

#endif
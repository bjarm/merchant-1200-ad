#ifndef MAPOBJECT_H
#define MAPOBJECT_H

#include <string>

#include <glew.h>
#include <SOIL.h>

#include "Shader.h"

class MapObject
{
public:
	GLfloat coordX, coordY, coordZ, objWidth, objHeight;
	GLuint VBO, VAO, EBO, texture;
	char* texturePath;
	Shader objShader = Shader("shader.vs", "shader.frag");


	MapObject(GLfloat x, GLfloat y, GLfloat z, GLfloat width, GLfloat height, char* path);

	void drawObject();

	void cleanObject();
};

#endif
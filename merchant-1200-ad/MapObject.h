#ifndef MAPOBJECT_H
#define MAPOBJECT_H

#include <string>

#include <glew.h>
#include <glfw3.h>
#include <SOIL.h>

#include "Shader.h"

class MapObject
{
public:
	GLfloat coordX, coordY, coordZ, objWidth, objHeight;
	GLuint VBO, VAO, EBO, texture;
	char* texturePath;
	std::string objName;
	Shader objShader = Shader("shader.vs", "shader.frag");

	MapObject();
	MapObject(GLfloat x, GLfloat y, GLfloat z, GLfloat width, GLfloat height, char* path, std::string name);

	bool isInsideRectangle(double x0, double y0);

	void drawObject();

	void cleanObject();
};

#endif
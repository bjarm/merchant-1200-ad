#include "MapObject.h"

MapObject::MapObject(GLfloat x, GLfloat y, GLfloat z, GLfloat width, GLfloat height, char* path, std::string name)
{
	coordX = x / 600;
	coordY = y / 450;
	coordZ = z;
	objWidth = width / 600;		// Replace with parcer/libconfig
	objHeight = height / 450;
	texturePath = path;
	objName = name;

	GLfloat vertices[] = {
		 // Position											    // Color			// Texture
		 (coordX + objWidth / 2),  (coordY - objHeight / 2), coordZ,  1.0f, 0.0f, 0.0f,   1.0f, 1.0f,   // Top right
		 (coordX + objWidth / 2),  (coordY + objHeight / 2), coordZ,  0.0f, 1.0f, 0.0f,   1.0f, 0.0f,   // Bottom right
		 (coordX - objWidth / 2),  (coordY + objHeight / 2), coordZ,  0.0f, 0.0f, 1.0f,   0.0f, 0.0f,   // Botttom left
	 	 (coordX - objWidth / 2),  (coordY - objHeight / 2), coordZ,  1.0f, 1.0f, 0.0f,   0.0f, 1.0f    // Top left
	};

	GLuint indices[] = {
		0, 1, 3,
		1, 2, 3
	};

	glGenVertexArrays(1, &VAO);
	glGenBuffers(1, &VBO);
	glGenBuffers(1, &EBO);

	glBindVertexArray(VAO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)0);
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)(3 * sizeof(GLfloat)));
	glEnableVertexAttribArray(1);
	glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)(6 * sizeof(GLfloat)));
	glEnableVertexAttribArray(2);

	glBindVertexArray(0);

	int tWidth, tHeight;

	glGenTextures(1, &texture);
	glBindTexture(GL_TEXTURE_2D, texture);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);

	unsigned char* image = SOIL_load_image(texturePath, &tWidth, &tHeight, 0, SOIL_LOAD_RGBA);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, tWidth, tHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, image);
	glGenerateMipmap(GL_TEXTURE_2D);

	SOIL_free_image_data(image);
	glBindTexture(GL_TEXTURE_2D, 0);
}

void MapObject::drawObject()
{
	glActiveTexture(GL_TEXTURE0);
	glTexEnvi(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	glBindTexture(GL_TEXTURE_2D, texture);
	glUniform1i(glGetUniformLocation(objShader.Program, "texture"), 0);

	objShader.Use();

	glBindVertexArray(VAO);
	glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
	glBindVertexArray(0);
}

void MapObject::cleanObject()
{
	glDeleteVertexArrays(1, &VAO);
	glDeleteBuffers(1, &VBO);
	glDeleteBuffers(1, &EBO);
}

bool MapObject::isInsideRectangle(double x0, double y0) {

	double x, y, width, height;
	if (coordX < 0)
		x = (coordX + 1) * 600;
	else
		x = (coordX + 1) * 600;
	if (coordY < 0)
		y = (coordY * -1 + 1) * 450;
	else
		y = (1 - coordY) * 450;

	width = objWidth * 600 / 2;
	height = objHeight * 450 / 2;


	return (x0 < x + width && x0 > x - width && y0 < y + height && y0 > y - height);
}
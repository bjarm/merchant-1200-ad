#include "MapObject.h"

MapObject::MapObject(GLfloat x, GLfloat y, GLfloat width, GLfloat height, char* path)
{
	coordX = x;
	coordY = y;
	objWidth = width;
	objHeight = height;
	texturePath = path;

	GLfloat vertices[] = {
		// Позиции													// Текстурные координаты
		 (coordX + objWidth / 2),  (coordY - objHeight / 2), 0.0f,  1.0f, 0.0f, 0.0f,   1.0f, 1.0f,   // Верхний правый
		 (coordX + objWidth / 2),  (coordY + objHeight / 2), 0.0f,  0.0f, 1.0f, 0.0f,   1.0f, 0.0f,   // Нижний правый
		 (coordX - objWidth / 2),  (coordY + objHeight / 2), 0.0f,  0.0f, 0.0f, 1.0f,   0.0f, 0.0f,   // Нижний левый
	 	 (coordX - objWidth / 2),  (coordY - objHeight / 2), 0.0f,  1.0f, 1.0f, 0.0f,   0.0f, 1.0f    // Верхний левый
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

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);
	float borderColor[] = { 1.0f, 1.0f, 0.0f, 1.0f };
	glTexParameterfv(GL_TEXTURE_2D, GL_TEXTURE_BORDER_COLOR, borderColor);

	unsigned char* image = SOIL_load_image(texturePath, &tWidth, &tHeight, 0, SOIL_LOAD_RGB);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, tWidth, tHeight, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
	glGenerateMipmap(GL_TEXTURE_2D);

	SOIL_free_image_data(image);
	glBindTexture(GL_TEXTURE_2D, 0);
}

void MapObject::drawObject()
{
	glActiveTexture(GL_TEXTURE0);
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
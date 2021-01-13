#define GLEW_STATIC

#include <iostream>
#include <vector>

#include <glew.h>
#include <glfw3.h>
#include <SOIL.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include "Shader.h"
#include "MapObject.h"
#include "City.h"
#include "game.h"

void key_callback(GLFWwindow* window, int key, int scancode, int action, int mode);
void mouse_button_callback(GLFWwindow* window, int button, int action, int mods);

int WIDTH = 1200;
int HEIGHT = 900;

std::vector<City> cities;

int main()
{	
	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3); 
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
	glfwWindowHint(GLFW_RESIZABLE, GL_FALSE);
	
	GLFWwindow* window = glfwCreateWindow(WIDTH, HEIGHT, "XIII Merchant", nullptr, nullptr);
	if (window == nullptr)
	{
		std::cout << "Failed to create GLFW window" << std::endl;
		glfwTerminate();
		return -1;
	}
	glfwMakeContextCurrent(window);
	glfwSetKeyCallback(window, key_callback);
	glfwSetMouseButtonCallback(window, mouse_button_callback);

	glewExperimental = GL_TRUE;
	if (glewInit() != GLEW_OK)
	{
		std::cout << "Failed to initialize GLEW" << std::endl;
		return -1;
	}
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glViewport(0, 0, WIDTH, HEIGHT);
	
	// Create screen objects
	MapObject Map((GLfloat)0.0f, (GLfloat)0.0f, (GLfloat)0.0f, (GLfloat)WIDTH, (GLfloat)HEIGHT, (char*)"map.png");
	City Novgorod((GLfloat)125.0f, (GLfloat)50.0f, (GLfloat)-1.0f, (GLfloat)120, (GLfloat)75, (char*)"large_city.png");
	cities.push_back(Novgorod);

	while (!glfwWindowShouldClose(window))
	{
		glfwPollEvents();

		glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		Map.drawObject();
		Novgorod.drawObject();

		glfwSwapBuffers(window);
	}

	Map.cleanObject();
	Novgorod.cleanObject();

	glfwTerminate();
	return 0;
}

bool isInsideRectangle(double x0, double y0, City city) {

	double x, y, width, height;
	if (city.coordX < 0)
		x = (city.coordX + 1) * 600;
	else 
		x = (city.coordX + 1)  * 600;
	if (city.coordY < 0)
		y = (city.coordY * -1 + 1) * 450;
	else
		y = (1 - city.coordY) * 450;

	width = city.objWidth * 600 / 2;
	height = city.objHeight * 450 / 2;

	return (x0 < x + width && x0 > x - width && y0 < y + height && y0 > y - height);
}

void key_callback(GLFWwindow* window, int key, int scancode, int action, int mode)
{
	if (key == GLFW_KEY_ESCAPE && action == GLFW_PRESS)
		glfwSetWindowShouldClose(window, GL_TRUE);
}

void mouse_button_callback(GLFWwindow* window, int button, int action, int mods)
{
	if (button == GLFW_MOUSE_BUTTON_LEFT && action == GLFW_PRESS) {
		double xpos, ypos;
		glfwGetCursorPos(window, &xpos, &ypos);
		for (City city : cities) {
			if (isInsideRectangle(xpos, ypos, city))
				std::cerr << "it is city" << std::endl;
		}
	}
	if (button == GLFW_MOUSE_BUTTON_RIGHT && action == GLFW_PRESS) {
		double xpos, ypos;
		glfwGetCursorPos(window, &xpos, &ypos);
		for (City city : cities) {
			if (isInsideRectangle(xpos, ypos, city))
				std::cerr << "GO to this city" << std::endl;
		}
	}
}

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
#include "Scene.h"

void key_callback(GLFWwindow* window, int key, int scancode, int action, int mode);
void mouse_button_callback(GLFWwindow* window, int button, int action, int mods);

int WIDTH = 1200;
int HEIGHT = 900;

enum Scenes
{
	MAP_SCENE,
	CITY_SCENE,
};

std::vector<City> cities;
Scene mainScene;
Scene mapScene;
Scene winScene;

Scenes activeScene;

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
	
	// cities vector;
	
	// Create screen objects
	MapObject map((GLfloat)0.0f, (GLfloat)0.0f, (GLfloat)0.0f, (GLfloat)WIDTH, (GLfloat)HEIGHT, (char*)"map.png", "MAP");
	mapScene.addObject(map);
	City novgorod((GLfloat)125.0f, (GLfloat)50.0f, (GLfloat)-0.1f, (GLfloat)120, (GLfloat)75, (char*)"large_city.png", "NOVGOROD");
	mapScene.addObject(novgorod);
	cities.push_back(novgorod);


	MapObject cityWindow((GLfloat)0.0f, (GLfloat)0.0f, (GLfloat)-0.2f, (GLfloat)600, (GLfloat)450, (char*)"city_window.png", "WINDOW_BACKGROUND");
	MapObject cityWindowExitButton((GLfloat)275.0f, (GLfloat)200.0f, (GLfloat)-0.25f, (GLfloat)30, (GLfloat)30, (char*)"button_exit.png", "WINDOW_EXIT");

	winScene.addObject(cityWindow);
	winScene.addObject(cityWindowExitButton);

	mainScene = mapScene;
	activeScene = MAP_SCENE;

	while (!glfwWindowShouldClose(window))
	{
		glfwPollEvents();

		glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		for (MapObject object : mainScene.peekScene()) 
		{
			object.drawObject();
		}
		glfwSwapBuffers(window);
	}
	for (MapObject object : mainScene.peekScene())
	{
		object.cleanObject();
	}

	glfwTerminate();
	return 0;
}

bool isInsideRectangle(double x0, double y0, MapObject city) {

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
	if (button == GLFW_MOUSE_BUTTON_LEFT && action == GLFW_PRESS && activeScene == MAP_SCENE) 
	{
		double xpos, ypos;
		glfwGetCursorPos(window, &xpos, &ypos);

		for (City city : cities) {

			if (isInsideRectangle(xpos, ypos, city)) 
			{
				mainScene = winScene;
				activeScene = CITY_SCENE;
			}
		}
	}

	if (button == GLFW_MOUSE_BUTTON_LEFT && action == GLFW_PRESS && activeScene == CITY_SCENE)
	{
		double xpos, ypos;
		glfwGetCursorPos(window, &xpos, &ypos);

		for (MapObject object : mainScene.peekScene())
		{
			if (object.objName == "WINDOW_EXIT" && isInsideRectangle(xpos, ypos, object))
			{
				mainScene = mapScene;
				activeScene = MAP_SCENE;
			}
		}
	}
}

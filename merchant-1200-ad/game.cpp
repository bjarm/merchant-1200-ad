#define GLEW_STATIC
#define IMGUI_IMPL_OPENGL_LOADER_GLEW

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
	
	// Create screen objects
	MapObject map((GLfloat)0.0f, (GLfloat)0.0f, (GLfloat)0.0f, (GLfloat)WIDTH, (GLfloat)HEIGHT, (char*)"map.png", "MAP");
	mapScene.addObject(map);
	City novgorod((GLfloat)125.0f, (GLfloat)50.0f, (GLfloat)-0.1f, (GLfloat)120, (GLfloat)75, (char*)"large_city.png", "NOVGOROD");
	mapScene.addObject(novgorod);
	cities.push_back(novgorod);

	mainScene = mapScene;
	activeScene = MAP_SCENE;

	while (!glfwWindowShouldClose(window))
	{
		glfwPollEvents();

		glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		
		mainScene.drawScene();
		glfwSwapBuffers(window);
	}
	
	mainScene.cleanScene();

	glfwTerminate();
	return 0;
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

			if (city.isInsideRectangle(xpos, ypos)) 
			{
				Scene winScene;

				MapObject cityWindow((GLfloat)0.0f, (GLfloat)0.0f, (GLfloat)-0.2f, (GLfloat)600, (GLfloat)450, (char*)"city_window.png", "WINDOW_BACKGROUND");
				MapObject cityWindowExitButton((GLfloat)275.0f, (GLfloat)200.0f, (GLfloat)-0.25f, (GLfloat)30, (GLfloat)30, (char*)"button_exit.png", "WINDOW_EXIT");

				winScene.addObject(cityWindow);
				winScene.addObject(cityWindowExitButton);

				std::string cityname = city.objName;

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
			if (object.objName == "WINDOW_EXIT" && object.isInsideRectangle(xpos, ypos))
			{
				mainScene = mapScene;
				activeScene = MAP_SCENE;
			}
		}
	}
}

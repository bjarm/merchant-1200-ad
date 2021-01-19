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
#include "audio.h"

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

Audio sound;

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
	glEnable(GL_DEPTH_TEST);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glViewport(0, 0, WIDTH, HEIGHT);
	
	// Create screen objects
	// Creating map
	MapObject map((GLfloat)0.0f, (GLfloat)0.0f, (GLfloat)0.0f, (GLfloat)WIDTH, (GLfloat)HEIGHT, (char*)"map_filled.png", "MAP");
	mapScene.addObject(map);
	// Creating large cities
	City novgorod((GLfloat)100.0f, (GLfloat)35.0f, (GLfloat)-0.1f, (GLfloat)110, (GLfloat)65, (char*)"large_city.png", "NOVGOROD");
	mapScene.addObject(novgorod);
	City pskov((GLfloat)-30.0f, (GLfloat)-75.0f, (GLfloat)-0.1f, (GLfloat)110, (GLfloat)65, (char*)"large_city.png", "PSKOV");
	mapScene.addObject(pskov);
	City narva((GLfloat)-35.0f, (GLfloat)115.0f, (GLfloat)-0.1f, (GLfloat)110, (GLfloat)65, (char*)"large_city.png", "NARVA");
	mapScene.addObject(narva);
	City toropetz((GLfloat)150.0f, (GLfloat)-180.0f, (GLfloat)-0.1f, (GLfloat)110, (GLfloat)65, (char*)"large_city.png", "TOROPETZ");
	mapScene.addObject(toropetz);
	City vitebsk((GLfloat)120.0f, (GLfloat)-350.0f, (GLfloat)-0.1f, (GLfloat)110, (GLfloat)65, (char*)"large_city.png", "VITEBSK");
	mapScene.addObject(vitebsk);
	City polotzk((GLfloat)-40.0f, (GLfloat)-265.0f, (GLfloat)-0.1f, (GLfloat)110, (GLfloat)65, (char*)"large_city.png", "POLOTZK");
	mapScene.addObject(polotzk);
	City riga((GLfloat)-310.0f, (GLfloat)-170.0f, (GLfloat)-0.1f, (GLfloat)110, (GLfloat)65, (char*)"large_city.png", "RIGA");
	mapScene.addObject(riga);
	// Creating small cities
	City ladoga((GLfloat)195.0f, (GLfloat)185.0f, (GLfloat)-0.1f, (GLfloat)70, (GLfloat)40, (char*)"small_city.png", "LADOGA");
	mapScene.addObject(ladoga);
	City rusa((GLfloat)110.0f, (GLfloat)-40.0f, (GLfloat)-0.1f, (GLfloat)70, (GLfloat)40, (char*)"small_city.png", "RUSA");
	mapScene.addObject(rusa);
	City luki((GLfloat)90.0f, (GLfloat)-240.0f, (GLfloat)-0.1f, (GLfloat)70, (GLfloat)40, (char*)"small_city.png", "LUKI");
	mapScene.addObject(luki);
	City torzhok((GLfloat)390.0f, (GLfloat)-150.0f, (GLfloat)-0.1f, (GLfloat)70, (GLfloat)40, (char*)"small_city.png", "TORZHOK");
	mapScene.addObject(torzhok);
	City rzhev((GLfloat)360.0f, (GLfloat)-240.0f, (GLfloat)-0.1f, (GLfloat)70, (GLfloat)40, (char*)"small_city.png", "RZHEV");
	mapScene.addObject(rzhev);
	City druzk((GLfloat)-150.0f, (GLfloat)-300.0f, (GLfloat)-0.1f, (GLfloat)70, (GLfloat)40, (char*)"small_city.png", "DRUZK");
	mapScene.addObject(druzk);
	City izborsk((GLfloat)-145.0f, (GLfloat)-110.0f, (GLfloat)-0.1f, (GLfloat)70, (GLfloat)40, (char*)"small_city.png", "IZBORSK");
	mapScene.addObject(izborsk);
	City derpth((GLfloat)-170.0f, (GLfloat)0.0f, (GLfloat)-0.1f, (GLfloat)70, (GLfloat)40, (char*)"small_city.png", "DERPTH");
	mapScene.addObject(derpth);
	City vezenberg((GLfloat)-185.0f, (GLfloat)100.0f, (GLfloat)-0.1f, (GLfloat)70, (GLfloat)40, (char*)"small_city.png", "VEZENBERG");
	mapScene.addObject(vezenberg);
	City reval((GLfloat)-285.0f, (GLfloat)115.0f, (GLfloat)-0.1f, (GLfloat)70, (GLfloat)40, (char*)"small_city.png", "REVAL");
	mapScene.addObject(reval);
	
	/*MapObject mainCharacter((GLfloat)120.0f, (GLfloat)40.0f, (GLfloat)-0.11f, (GLfloat)50, (GLfloat)75, (char*)"mc.png", "MAIN_CHARACTER");
	mapScene.addObject(mainCharacter);*/

	cities.insert(cities.end(), { novgorod, pskov, narva, toropetz, vitebsk, polotzk, riga,
		ladoga, rusa, luki, torzhok, rzhev, druzk, izborsk, derpth, vezenberg, reval });
	
	mainScene = mapScene;
	activeScene = MAP_SCENE;

	sound.playSound(SOUND_MAIN, 0.5f, true);

	while (!glfwWindowShouldClose(window))
	{
		glfwPollEvents();

		glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		
		mainScene.drawScene();
		glfwSwapBuffers(window);
	}
	
	sound.stopSound();
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
	double xpos, ypos;
	glfwGetCursorPos(window, &xpos, &ypos);

	if (button == GLFW_MOUSE_BUTTON_LEFT && action == GLFW_PRESS && activeScene == MAP_SCENE) 
	{
		for (City city : cities) {

			if (city.isInsideRectangle(xpos, ypos)) 
			{
				Scene winScene;

				MapObject cityWindow((GLfloat)0.0f, (GLfloat)0.0f, (GLfloat)-0.2f, (GLfloat)900, (GLfloat)650, (char*)"city_window.png", "WINDOW_BACKGROUND");
				MapObject cityWindowExitButton((GLfloat)420.0f, (GLfloat)295.0f, (GLfloat)-0.25f, (GLfloat)30, (GLfloat)30, (char*)"button_exit.png", "WINDOW_EXIT");

				winScene.addObject(cityWindow);
				winScene.addObject(cityWindowExitButton);

				std::string cityname = city.objName;

				sound.playSound(SOUND_CITY, 1.0f, true);

				mainScene = winScene;
				activeScene = CITY_SCENE;
			}
		}
	}

	if (button == GLFW_MOUSE_BUTTON_LEFT && action == GLFW_PRESS && activeScene == CITY_SCENE)
	{
		for (MapObject object : mainScene.peekScene())
		{
			if (object.objName == "WINDOW_EXIT" && object.isInsideRectangle(xpos, ypos))
			{
				mainScene = mapScene;
				activeScene = MAP_SCENE;

				sound.stopSound(SOUND_CITY);
			}
		}
	}
	if (button == GLFW_MOUSE_BUTTON_RIGHT && action == GLFW_PRESS && activeScene == MAP_SCENE)
	{
		for (MapObject object : mainScene.peekScene())
		{
			double clickX, clickY;
			clickX = xpos;
			clickY = ypos;
			if (object.objName == "MAIN_CHARACTER")
			{
				//object.moveObject(clickX, clickY, 0.01f);
			}
		}
	}
}

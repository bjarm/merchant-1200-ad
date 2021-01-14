#ifndef SCENE_H
#define SCENE_H

#include <string>
#include <vector>

#include <glew.h>
#include <SOIL.h>

#include "Shader.h"
#include "MapObject.h"

class Scene
{
public:
	
	Scene();

	void addObject(MapObject newObject);

	void drawScene();

	void cleanScene();

	std::vector<MapObject> peekScene();

private:
	std::vector<MapObject> sceneObjects;
};

#endif
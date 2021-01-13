#include "Scene.h"

Scene::Scene()
{

}

void Scene::addObject(MapObject newObject)
{
	sceneObjects.push_back(newObject);
}

std::vector<MapObject> Scene::peekScene()
{
	return sceneObjects;
}
#include "Scene.h"

Scene::Scene()
{

}

void Scene::addObject(MapObject newObject)
{
	sceneObjects.push_back(newObject);
}

void Scene::drawScene() {
	for (MapObject object : sceneObjects)
	{
		object.drawObject();
	}
}

void Scene::cleanScene() {
	for (MapObject object : sceneObjects)
	{
		object.cleanObject();
	}
}

std::vector<MapObject> Scene::peekScene()
{
	return sceneObjects;
}
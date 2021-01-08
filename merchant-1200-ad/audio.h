#pragma once

#include "audiere.h"
#include <vector>


#define SOUND_BOWS     "sounds/sound2.mp3"
#define SOUND_CROSSBOW "sounds/sound1.mp3"

struct line{
	audiere::OutputStreamPtr sound;
	std::string filename;
};

class Audio
{
public:
	Audio();
	~Audio();
	void playSound(std::string filename);
	void stopAllSound();
private:
	audiere::AudioDevicePtr device = audiere::OpenDevice();
	std::vector <line> sounds;
};


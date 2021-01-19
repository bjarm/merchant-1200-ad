#pragma once

#include "audiere.h"
#include <vector>

constexpr auto SOUND_MAIN = "sounds/main.mp3";
constexpr auto SOUND_CITY = "sounds/sound4.mp3";
constexpr auto SOUND_BOWS = "sounds/sound2.mp3";
constexpr auto SOUND_CROSSBOW = "sounds/sound1.mp3";


struct line{
	audiere::OutputStreamPtr sound;
	std::string filename;
};

class Audio
{
public:
	Audio();
	~Audio();
	void playSound(std::string filename, float volume, bool repited);
	void stopSound(std::string filename = "");
private:
	audiere::AudioDevicePtr device = audiere::OpenDevice();
	std::vector <line> sounds;
};


#pragma comment(lib, "audiere.lib")

#include "audio.h"
#include <iostream>

Audio::Audio() {

}

Audio::~Audio() {

}

void Audio::playSound(std::string filename, float volume, bool repited) {

	if (!device) {
		std::cerr << "unable to connect to audio output device";
	}

	for (int k = 0; k < sounds.size(); k++) {

		if (sounds[k].filename == filename) {
			if(sounds[k].sound->isPlaying())
				sounds[k].sound->stop();
			sounds[k].sound->play();
			sounds[k].sound->setVolume(volume);
			sounds[k].sound->setRepeat(repited);
			return;
		}
	}

	audiere::OutputStreamPtr sound(OpenSound(device, filename.c_str(), true));

	line line1;
	line1.sound = sound;
	line1.filename = filename;

	if (!sound) {
		std::cout << "error of open file" << std::endl;
		return;
	}
	sound->play();
	sound->setVolume(volume);
	sound->setRepeat(repited);
	sounds.push_back(line1);
}

void Audio::stopSound(std::string filename) {

	if(!filename.size())
		for (int k = 0; k < sounds.size(); k++) {
			if (sounds[k].sound->isPlaying())
				sounds[k].sound->stop();
		}
	for (int k = 0; k < sounds.size(); k++) {
		if (sounds[k].filename == filename)
			sounds[k].sound->stop();
	}

	
}


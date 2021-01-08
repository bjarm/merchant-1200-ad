#pragma comment(lib, "audiere.lib")

#include "audio.h"
#include <iostream>

Audio::Audio() {

}

Audio::~Audio() {

}

void Audio::playSound(std::string filename) {

	if (!device) {
		std::cerr << "unable to connect to audio output device";
	}

	audiere::OutputStreamPtr sound(OpenSound(device, filename.c_str(), false));

	sound->play();
	sound->setVolume(1.0f);
	sounds.push_back(sound);
}

void Audio::stopAllSound() {

	for (int k = 0; k < sounds.size(); k++) {

		sounds[k]->stop();
	
	}
}

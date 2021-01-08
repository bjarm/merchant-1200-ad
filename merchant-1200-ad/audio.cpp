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

	for (int k = 0; k < sounds.size(); k++) {

		if (sounds[k].filename == filename) {
			sounds[k].sound->stop();
			sounds[k].sound->play();
			return;
		}
	}

	audiere::OutputStreamPtr sound(OpenSound(device, filename.c_str(), false));

	line line1;
	line1.sound = sound;
	line1.filename = filename;

	sound->play();
	sound->setVolume(1.0f);
	sounds.push_back(line1);
}

void Audio::stopAllSound() {

	for (int k = 0; k < sounds.size(); k++) {

		sounds[k].sound->stop();
	
	}
}


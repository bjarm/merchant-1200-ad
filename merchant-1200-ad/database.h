#ifndef DATABASE_H
#define DATABASE_H
#pragma once

#include "sqlite3.h";
#include <iostream>

#define DATABASE_NAME "gameDB.db"

#define ECONOMYC_TABLE_NAME "economyc"
#define ECONOMYC_CITY_NAME  "city"
#define ECONOMYC_GOLD_COUNT "gold"
#define ECONOMYC_T1_COUNT   "t1"
#define ECONOMYC_T2_COUNT   "t2"
#define ECONOMYC_T3_COUNT   "t3"
#define ECONOMYC_T4_COUNT   "t4"
#define ECONOMYC_T5_COUNT   "t5"

class DataBase{
public:
	DataBase();
	~DataBase();
	void connectToDataBase();
	void closeDataBase();
	bool insertIntoEconomycDataBase(std::string city, int gold, int t1, int t2, int t3, int t4, int t5);
	bool getInfFromEconomycDataBase(std::string city, int t[6]);
	bool changeInfFromEconomycDataBase(std::string city, std::string t, int positive, int negative);
	void showEconomycDataBase();
private:
	sqlite3* db;
	sqlite3_stmt* stmt;
	char* err;
private:
	bool openDataBase();
	bool restoreDataBase();
	bool createEconomycDataBase();
};

#endif // DATABASE_H
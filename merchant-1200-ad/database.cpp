#include "database.h"
#include <iostream>
#include <fstream>
#include <string>

DataBase::DataBase() {
}
DataBase::~DataBase() {
}

void DataBase::connectToDataBase() {
	std::ifstream file;
	file.open(DATABASE_NAME);

	if (!file) {
		this->restoreDataBase();
	}
	else {
		this->openDataBase();
	}

}

bool DataBase::restoreDataBase() {
	if (this->openDataBase()) {
		if (!this->createEconomycDataBase())
			return false;
		else
			return true;
	}
	else {
		std::cerr << "error:" << "failed to restore database " << DATABASE_NAME ;
		return false;
	}
	return false;
}

bool DataBase::openDataBase() {
	
	if (!sqlite3_open("gameDB.db", &db))
		return true;
	else
		return false;
}

void DataBase::closeDataBase() {
	sqlite3_close(db);
}
bool DataBase::createEconomycDataBase() {
	int rc = sqlite3_exec(db, "CREATE TABLE IF NOT EXISTS economyc(city varchar(100), gold INT, t1 INT, t2 INT, t3 INT, t4 INT, t5 INT );", NULL, NULL, &err);

	if (rc != SQLITE_OK) {
		std::cerr << "error of create " << ECONOMYC_TABLE_NAME << std::endl;
		std::cerr << sqlite3_errmsg(db);
		return false;
	}
	else
		return true;

}

bool DataBase::insertIntoEconomycDataBase(std::string city, int gold, int t1, int t2, int t3, int t4, int t5) {

	std::string query = "INSERT INTO " ECONOMYC_TABLE_NAME " VALUES ('"+ city +"', " + std::to_string(gold) + ", " + std::to_string(t1) + ", " + std::to_string(t2) + ", " + std::to_string(t3) + ", " + std::to_string(t4) + ", " + std::to_string(t5) + ")";

	int rc = sqlite3_exec(db, query.c_str(), NULL, NULL, &err);

	if (rc != SQLITE_OK) {
		std::cerr << "error of insert to " << ECONOMYC_TABLE_NAME << std::endl;
		std::cerr << sqlite3_errmsg(db);
		return false;
	}
	else
		return true;
}

void DataBase::showEconomycDataBase() {
	sqlite3_prepare_v2(db, "SELECT city, gold, t1, t2, t3, t4, t5 FROM economyc", -1, &stmt, 0);
	while (sqlite3_step(stmt)){
		if (!sqlite3_column_text(stmt, 0))
			break;
		std::cerr << sqlite3_column_text(stmt, 0)
				  << " " << sqlite3_column_int(stmt, 1)
				  << " " << sqlite3_column_int(stmt, 2)
				  << " " << sqlite3_column_int(stmt, 3)
				  << " " << sqlite3_column_int(stmt, 4)
				  << " " << sqlite3_column_int(stmt, 5)
				  << " " << sqlite3_column_int(stmt, 6) << std::endl;
	}
	sqlite3_finalize(stmt);
}

bool DataBase::getInfFromEconomycDataBase(std::string city, int t[6]) {
	std::string query = "SELECT city, gold, t1, t2, t3, t4, t5 FROM economyc WHERE city = '" + city + "'";

	sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, 0);

	sqlite3_step(stmt);

	if (!sqlite3_column_text(stmt, 0)) {
		sqlite3_finalize(stmt);
		return false;
	}

	t[0] = sqlite3_column_int(stmt, 1);
	t[1] = sqlite3_column_int(stmt, 2);
	t[2] = sqlite3_column_int(stmt, 3);
	t[3] = sqlite3_column_int(stmt, 4);
	t[4] = sqlite3_column_int(stmt, 5);
	t[5] = sqlite3_column_int(stmt, 6);

	sqlite3_finalize(stmt);

	return true;
}

bool DataBase::changeInfFromEconomycDataBase(std::string city, std::string t, int positive, int negative) {
	std::string query = "SELECT city, " + t + " FROM economyc WHERE city = '" + city + "'";

	sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, 0);

	sqlite3_step(stmt);

	if (!sqlite3_column_text(stmt, 0))
		return false;
	else
		if (sqlite3_column_int(stmt, 1) < negative) {
			std::cerr << "This change is imposible, value can not be less than 0";
			return false;
		}
	
	query = "UPDATE economyc SET " + t + " = " + std::to_string(sqlite3_column_int(stmt, 1) + positive - negative) + " WHERE city = '" + city + "'";

	int rc = sqlite3_exec(db, query.c_str(), NULL, NULL, &err);

	if (rc != SQLITE_OK) {
		std::cerr << "error of update " << t << "from " << ECONOMYC_TABLE_NAME << std::endl;
		std::cerr << sqlite3_errmsg(db) << std::endl;
		sqlite3_finalize(stmt);
		return false;
	}
	else {
		sqlite3_finalize(stmt);
		return true;
	}
}




#include <iostream>
#include <fstream>
#include <map>
#include "WordsFrequencyDictionary.h"
#include "MapFrequencyDictionary.h"
#include "WordsDictionaryMaker.h"
#include <thread>
#include <mutex>

using namespace std;
mutex mtx;

void ThreadOne() {
	clock_t t = clock();


	//const string text_sample = "1z.txt";
	WordsDictionaryMaker maker(new MapFrequencyDictionary());
	maker.Process("1z.txt");
	maker.Save(maker.GetDictionary()->GetTitle() + "1.txt");



	
	t = clock() - t;
	double time_taken = ((double)t) / CLOCKS_PER_SEC;
	cout << "Time taken: " << time_taken << endl << endl;
	cout << "ID thread = " << this_thread::get_id() <<"----->\tThreadOne\t"<< endl;
}


void ThreadTwo() {
	clock_t t = clock();


	//const string text_sample = "2z.txt";
	WordsDictionaryMaker maker(new MapFrequencyDictionary());
	maker.Process("2z.txt");
	maker.Save(maker.GetDictionary()->GetTitle() + "2.txt");




	t = clock() - t;
	double time_taken = ((double)t) / CLOCKS_PER_SEC;
	cout << "Time taken: " << time_taken << endl << endl;
	cout << "ID thread = " << this_thread::get_id() << "----->\tThreadTwo\t" << endl;
}


void ThreadThree() {
	clock_t t = clock();


	FILE* file1 = fopen("1.txt", "r");
	FILE* file2 = fopen("2.txt", "r");
	FILE* file3 = fopen("final.txt", "w");
	if (file1 != nullptr && file2 != nullptr)
	{
		char buffer1[80];
		while (!feof(file1))
		{
			strcpy(buffer1, "");
			fgets(buffer1, 80, file1);
			fputs(buffer1, file2);
			fputs(buffer1, file3);
		}
		fclose(file1);
		fclose(file2);
		fclose(file3);
	}
	else
	{
		cout << "Error opening file!" << endl;
	}




	t = clock() - t;
	double time_taken = ((double)t) / CLOCKS_PER_SEC;
	cout << "Time taken: " << time_taken << endl << endl;
	cout << "ID thread = " << this_thread::get_id() << "----->\tThreadTwo\t" << endl;
}


void split()
{
	mtx.lock();
	FILE* File = fopen("1z.txt", "rb");
	if (File != nullptr)
	{
		fseek(File, 0, SEEK_END);
		size_t fileSize = ftell(File);
		fseek(File, 0, SEEK_SET);
		char* buffer = new char[fileSize];

		size_t q = fileSize / 2;

		if (buffer != nullptr)
		{
			fread(buffer, sizeof(char), fileSize, File);
			fseek(File, 0, SEEK_SET);

			char* buffer1 = new char[q];
			fread(buffer1, sizeof(char), q, File);

			FILE* dstFile1 = fopen("One.txt", "wb");
			if (dstFile1 != nullptr)
			{
				fwrite(buffer1, sizeof(char), q, dstFile1);
				fclose(dstFile1);
			}

			delete[] buffer1;


			size_t lengthFile2 = fileSize - q;
			char* buffer2 = new char[lengthFile2];

			fread(buffer2, sizeof(char), lengthFile2, File);
			FILE* dstFile2 = fopen("Two.txt", "wb");
			if (dstFile2 != nullptr)
			{
				fwrite(buffer2, sizeof(char), lengthFile2, dstFile2);
				fclose(dstFile2);
			}

			delete[] buffer2;
		}

		delete[] buffer;


		fclose(File);
	}
	mtx.unlock();
}

void ThreadO() {
	mtx.lock();
	clock_t t = clock();


	const string text_sample = "One.txt";
	WordsDictionaryMaker maker(new MapFrequencyDictionary());
	maker.Process("One.txt");
	maker.Save(maker.GetDictionary()->GetTitle() + "1.txt");




	t = clock() - t;
	double time_taken = ((double)t) / CLOCKS_PER_SEC;
	cout << "Time taken: " << time_taken << endl << endl;
	cout << "ID thread = " << this_thread::get_id() << "----->\tThreadOne\t" << endl;
	mtx.unlock();
}
void ThreadT() {
	mtx.lock();
	clock_t t = clock();


	const string text_sample = "Two.txt";
	WordsDictionaryMaker maker(new MapFrequencyDictionary());
	maker.Process("Two.txt");
	maker.Save(maker.GetDictionary()->GetTitle() + "2.txt");




	t = clock() - t;
	double time_taken = ((double)t) / CLOCKS_PER_SEC;
	cout << "Time taken: " << time_taken << endl << endl;
	cout << "ID thread = " << this_thread::get_id() << "----->\tThreadOne\t" << endl;
	mtx.unlock();
}

void main()
{
	//-однопоточный
	/*cout <<"ID thread = "<< this_thread::get_id() <<"----->\tmain\t"<< endl;
	thread th(ThreadOne);	
	th.join();*/



	//-2 независимых потока, которые создают 2 независимых словаря, а потом их сливают в один
	/*cout << "ID thread = " << this_thread::get_id() << "----->\tmain\t" << endl;
	thread th(ThreadOne);
	thread tw(ThreadTwo);	
	thread tt(ThreadThree);
	th.join();
	tt.join();
	tw.join();*/

	//-2 потока, которые работают с одним словарём и используют критические секции
	
	
	thread th(split);
	thread tw(ThreadO);
	thread tt(ThreadT);
	thread ta(ThreadThree);
	th.join();
	tw.join();
	tt.join();
	ta.join();

	
}
#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>

int read_file(char const *fileName, std::vector<std::vector<int>> &C, int size)
{
    int value;
    std::string line;
    std::ifstream inFile(fileName);
    
    if (!inFile.is_open())
    {
        std::cerr << "Can't open input file" << std::endl;
     	return -1;   
    }
        
	for(int i = 0; i < size; ++i)
	{
		getline(inFile, line);
    	std::stringstream stream(line);
    	
    	C.push_back(std::vector<int>());
    	while(stream >> value)
    		C[i].push_back(value);
	}

    inFile.close();
}

int min(int a, int b, int c)
{
	if(a <= b && a <= c)
		return a;
	
	if(b <= a && b <= c)
		return b;
		
	return c;
}

int write_file(char const *fileName, int result)
{
	std::ofstream inFile(fileName);
    
    if (!inFile.is_open())
    {
        std::cerr << "Can't open input file" << std::endl;
     	return -1;   
    }
    
    inFile << result;
    
    inFile.close();
}

int main()
{
	int size = 8;
	std::vector<std::vector<int>> C;
	if(read_file("king2.in", C, size) == -1)
		return -1;
	
	for(int i = 0; i < size; ++i)
	{
		for(int j = 0; j < size; ++j)
		{
			std::cout << C[i][j] << "\t";
		}
		std::cout << "\n";
	}
	
	std::vector<std::vector<int>> g(size, std::vector<int>(size, 0));
	
	for(int i = size - 1; i >= 0; --i)
	{
		for(int j = 0; j < size; ++j) 
		{
			if(j == 0 && i != size - 1)
				g[i][j] = g[i + 1][j];
			else if(i == size - 1 && j != 0)
				g[i][j] = g[i][j - 1];
			else if(i != size - 1 && j != 0)
				g[i][j] = min(g[i][j - 1], g[i + 1][j], g[i + 1][j - 1]);
			
			g[i][j] += C[i][j];
		}
	}

//	std::cout << "\n";
//	for(int i = 0; i < size; ++i)
//	{
//		for(int j = 0; j < size; ++j)
//		{
//			std::cout << g[i][j] << "\t";
//		}
//		std::cout << "\n";
//	}
	
	if(write_file("king2.out", g[0][size - 1]) == -1)
		return -1;
	
	std::cout << g[0][size - 1] << std::endl;
	
	return 0;
}

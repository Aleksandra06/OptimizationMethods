#include <stdio.h>
#include <stdlib.h>

int main(){
	FILE* input, *output;
	int n, m;
	int i, j;
	
	input = fopen("knight.in", "r");
	fscanf(input, "%d %d", &n, &m);
	fclose(input);
	
	int** dp = (int**)calloc(n+1, sizeof(int**));
	for (i = 0; i < n+1; i++){
		dp[i] = (int*)calloc(m+1, sizeof(int**));
	}
	dp[1][1] = 1;
	
	for (i = 2; i <= n; i++){
		for (j = 2; j <=m; j++){
			dp[i][j] = dp[i-1][j-2]+dp[i-2][j-1];
		}
	}
	
	printf("\n%d\n", dp[n][m]);
	
	output = fopen("knight.out", "w");
	fprintf(output, "%d", dp[n][m]);
	fclose(output);
	
	
	for (i = 0; i <= n; i++){
		free(dp[i]);
	}
	free(dp);
	return 0;
}
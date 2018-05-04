#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>


int main(void)
{
	int num;

	printf("Welcome!\n");
	printf("Enter an integer number: ");
	scanf("%d", &num);


	if (num <= 99 && num > 5) {
		printf("Medium number!\n");
	}
	else if (num <= 5) {
		printf("Small number!\n");
	}
	else if (num > 99) {
		printf("Big number!\n"); 
	}

	printf("Goodbye!\n");

	return 0;

}
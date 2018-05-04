// EmployeeDataPractice.c

#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#define MAXEMP 4
struct Employee {
	int id;
	char name[31];
	float wage;
};

int main()
{
	struct Employee employeeData[MAXEMP][1] = { '\0'};
	int i, flag = 1;
	printf("Enter employee data as requested.\n");

	for (i = 0; i < MAXEMP && flag > 0; i++)
	{
		printf("Please enter id for employee %d.\n", i + 1);
		scanf(" %d", &employeeData[i][0].id);
		flag = employeeData[i][0].id;
		
		if(flag > 0) //we stop once we put in a zero, and the loop exits afterwards
		{ 
			printf("Please enter name for employee %d.\n", i + 1);
			scanf(" %[^\n]", employeeData[i][0].name);
			printf("Please enter wage for employee %d.\n", i + 1);
			scanf(" %f", &employeeData[i][0].wage);
		}
	}

	printf("Number Name         Salary\n");
	printf("--------------------------\n");
	for (i = MAXEMP - 1; i > -1; i--)
	{
		if(employeeData[i][0].id != 0)
		{ 
			printf("%05d %s         %.2f\n", employeeData[i][0].id, employeeData[i][0].name, employeeData[i][0].wage);
		}
	}
    return 0;
}


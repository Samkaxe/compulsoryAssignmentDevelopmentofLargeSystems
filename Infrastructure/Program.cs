// See https://aka.ms/new-console-template for more information

using Infrastructure;


OperationService operationService = new OperationService();
operationService.LogOperation("Addition", 2, 2, 4);
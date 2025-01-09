using System;

namespace todo.ErrorManagement;

public enum OperationsStatus
{
    // Usando http code para os códigos das operações
    Success = 200,
    NotFound = 404,
    InvalidEntry = 403
}

public static class OperationsErrorExtensions
{
    public static bool IsSuccess(this OperationsStatus status) => status == OperationsStatus.Success;        

    public static int GetErrorsCode(this OperationsStatus status) => (int)status;
}
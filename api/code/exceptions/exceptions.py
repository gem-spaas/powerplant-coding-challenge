from gems_core.exceptions.exceptions import DataException
from exceptions.error_codes import error_codes


class BSError(DataException):
    def __init__(self, code, arg=None):
        super(BSError, self).__init__(code=code, module='BS', module_error_codes=error_codes, arg=arg)


class SolverException(BSError):
    def __init__(self, arg):
        super(SolverException, self).__init__(code='BS_0001', arg=arg)

class PlannerException(BSError):
    def __init__(self, arg):
        super(PlannerException, self).__init__(code='BS_0002', arg=arg)

class BuilderException(BSError):
    def __init__(self, arg):
        super(BuilderException, self).__init__(code='BS_0003', arg=arg)        
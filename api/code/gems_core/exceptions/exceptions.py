from gems_core.exceptions.error_codes import error_codes


class DataException(Exception):
    def __init__(self, code, module, module_error_codes=None, arg=None):
        if module_error_codes is not None:
            error_codes.update(module_error_codes)
        self.code = code
        self.module = module
        self.arg = arg

    def __str__(self):
        text = f'Error: {self.code}. {self.message()} [{self.module}]'
        return text

    def message(self):
        if self.arg is not None:
            return error_codes[self.code].format(self.arg)
        else:
            return error_codes[self.code]


# Generic Errors

class GenericError(DataException):
    def __init__(self, code, arg=None):
        super(GenericError, self).__init__(code=code, module='GEN', arg=arg)


class BadRequestError(GenericError):
    def __init__(self, arg):
        super(BadRequestError, self).__init__(code='CORE_0001', arg=arg)


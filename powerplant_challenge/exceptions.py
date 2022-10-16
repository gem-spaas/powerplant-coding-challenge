from rest_framework.exceptions import APIException


class UnProcessableRequest(APIException):
    status_code = 422
    default_detail = "Unable to produce a production plan that fits the input"
    default_code = "unprocessable_request"

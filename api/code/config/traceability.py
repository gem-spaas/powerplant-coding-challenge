import logging
import uuid
import contextvars

# Context variable to store the correlation ID
correlation_id_ctx_var = contextvars.ContextVar("correlation_id", default=None)


def get_correlation_id():
    """
    Returns the current correlation ID.
    """
    return correlation_id_ctx_var.get()


def set_correlation_id(correlation_id):
    """
    Sets the correlation ID.
    """
    correlation_id_ctx_var.set(correlation_id)


def generate_correlation_id():
    """
    Generates a new correlation ID and sets it in the context variable.
    """
    correlation_id = str(uuid.uuid4())
    set_correlation_id(correlation_id)
    return correlation_id


class CorrelationIdFilter(logging.Filter):
    """
    Logging filter to add correlation ID to log records.
    """

    def filter(self, record):
        record.correlation_id = get_correlation_id() or "N/A"
        return True

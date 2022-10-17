import json

from asgiref.sync import async_to_sync
from channels.generic.websocket import WebsocketConsumer, AsyncWebsocketConsumer


class PlanConsumer(AsyncWebsocketConsumer):

    async def connect(self):
        await self.channel_layer.group_add(group="test", channel=self.channel_name)
        await self.accept()

    async def disconnect(self, code):  # changed
        await self.channel_layer.group_discard(group="test", channel=self.channel_name)
        await super().disconnect(code)

    async def plan_event(self, event):
        await self.send(event["message"].decode("utf8"))

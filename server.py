import http.server
 
PORT = 8888
server_address = ("", PORT)

server = http.server.HTTPServer
handler = http.server.CGIHTTPRequestHandler
handler.cgi_directories = ["/"]
print("Listening Port :", PORT)

httpd = server(server_address, handler)
httpd.serve_forever()

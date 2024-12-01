import random
import string
import datetime

methods = ["GET", "GET", "GET", "GET", "POST", "POST", "PUT", "DELETE", "HEAD"]
codes = [200, 200, 200, 303, 400, 401, 403, 404, 404, 404, 404, 500, 501, 503, 503]
n = 10000
with open(".log", 'w') as file:
    for i in range(n):
        resourse = ''.join(random.choices(string.ascii_letters, k=5))
        octets = [str(random.randint(0, 255)) for _ in range(4)]
        ip = ".".join(octets)

        file.write(f'{ip} - - [{datetime.datetime.now().isoformat()}] {random.choice(methods)} "/{resourse} HTTP/1.1" {random.choice(codes)}\n')


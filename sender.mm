#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <string>

void SendSpawn(const char* item, int count)
{
    int sock = socket(AF_INET, SOCK_DGRAM, 0);

    sockaddr_in addr{};
    addr.sin_family = AF_INET;
    addr.sin_port = htons(7777);
    addr.sin_addr.s_addr = inet_addr("127.0.0.1");

    std::string msg =
        "{\"type\":\"spawn\",\"item\":\"" +
        std::string(item) +
        "\",\"count\":" +
        std::to_string(count) + "}";

    sendto(sock, msg.c_str(), msg.size(), 0,
           (sockaddr*)&addr, sizeof(addr));

    close(sock);
}

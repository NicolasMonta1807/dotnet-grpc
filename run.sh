#!/bin/bash

echo "Ejecutando GrpcServer..."
cd ./GrpcServer
dotnet clean
dotnet build
dotnet run &
SERVER_PID=$!

sleep 3

echo "Ejecutando GrpcClient..."
cd ../GrpcClient
dotnet clean
dotnet build
dotnet run

echo "Deteniendo GrpcServer..."
kill $SERVER_PID

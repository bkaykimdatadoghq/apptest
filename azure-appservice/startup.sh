#!/bin/bash
# Datadog Agent startup script for Azure App Service (Linux)
# Set this as "Startup Command" in App Service Configuration > General settings

set -e

DD_AGENT_VERSION="7"
DD_INSTALL_DIR="/home/datadog"

echo "[startup] Installing Datadog Agent..."

mkdir -p "$DD_INSTALL_DIR"
cd "$DD_INSTALL_DIR"

# Download and install Datadog agent
DD_AGENT_MAJOR_VERSION=$DD_AGENT_VERSION \
DD_API_KEY=$DD_API_KEY \
DD_SITE=${DD_SITE:-"datadoghq.com"} \
bash -c "$(curl -L https://s3.amazonaws.com/dd-agent-distrib/scripts/install_script_agent7.sh)"

# Start Datadog agent in background
datadog-agent run &

echo "[startup] Datadog Agent started."

# Start the .NET app
echo "[startup] Starting .NET application..."
cd /home/site/wwwroot
dotnet SampleApp.dll

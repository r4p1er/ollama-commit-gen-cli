# Ollama Commit Gen CLI

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Releases](https://img.shields.io/github/v/release/r4p1er/ollama-commit-gen-cli)](https://github.com/r4p1er/ollama-commit-gen-cli/releases)

**Cross-platform CLI tool for automatic generation of meaningful and standardized Git commit messages using local LLMs (e.g., CodeLlama via Ollama).**

## About the Project

Writing good commit messages is often overlooked during software development. This CLI tool automates the process by generating clear, structured, and informative commit messages based on staged Git changes — all without sending your code to the cloud.

Key features:

  -  **Privacy-first**: Runs fully offline using locally deployed LLMs (via [Ollama](https://ollama.com/))
  -  **Customizable**: Choose generation language (eng, rus, etc.), commit style (Conventional Commits, Gitmoji, etc.), and model parameters (e.g., temperature, top_k)
  -  **Cross-platform**: Ready-to-use binaries for Windows, Linux, and macOS
  -  **Open-source**: Licensed under MIT

## Requirements

  * Installed and running [Ollama](https://ollama.com/) with a model such as `codellama` or `llama3`
  * Git repository with staged changes

## Installation

### Option 1: Download Prebuilt Binaries

  1. Go to the [latest release](https://github.com/r4p1er/ollama-commit-gen-cli/releases)
  2. Download the archive for your OS:
     - `OllamaCommitGenCli-x.y.z-windows-amd64.zip`
     - `OllamaCommitGenCli-x.y.z-linux-amd64.tar.gz`
     - `OllamaCommitGenCli-x.y.z-osx-amd64.tar.gz`
  3. Unpack the archive and move the binaries to a directory included in your system `PATH` (optional but recommended for global access)
  4. Rename the main file (OllamaCommitGen.Cli) so that it is easier for you to call it in the console

### Option 2: Build from Source

> Requires [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download) and Git.

```bash
git clone https://github.com/r4p1er/ollama-commit-gen-cli.git
cd ollama-commit-gen-cli
dotnet publish -c Release -r <your_runtime> --self-contained true
```

Replace `<your_runtime>` with:
  * `win-x64` for Windows
  * `linux-x64` for Linux
  * `osx-x64` for macOS

Published binaries will be in: `bin/Release/net8.0/<runtime>/publish/`

## Usage

Once installed, run the CLI inside your Git repository:
```bash
OllamaCommitGen.Cli
```

By default, the tool will:
1. Collect your staged changes (`git diff --cached`)
2. Send the diff to your local Ollama model
3. Generate a commit message according to your preferred style/language
4. Display the result in your terminal (optionally allowing edits)
5. Make commit with the generated message

### Options and Configuration

You can customize generation using various CLI flags or configuration file (config.json):
Examples:
```bash
OllamaCommitGen.Cli --lang eng --temperature 0.7
```

Or use a config file:
```json
{
    "origin": "http://localhost:11434",
    "model": "codellama",
    "lang": "eng",
    "keepAlive": "5m",
    "noStream": false,
    "example": "Feat: add create method to the users controller",
    "exampleDescription": "Use Conventional Commits format",
    "miroStat": 0,
    "miroStatEta": 0.1,
    "miroStatTau": 5,
    "numCtx": 2048,
    "repeatLastN": 64,
    "repeatPenalty": 1.1,
    "temperature": 0.8,
    "seed": 0,
    "stop": ["data", "file", "code"],
    "tfsZ": 1,
    "numPredict": 128,
    "topK": 40,
    "topP": 0.9
}
```

## License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).

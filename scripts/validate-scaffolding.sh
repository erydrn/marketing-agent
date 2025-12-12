#!/bin/bash
# Validation script for project scaffolding
# This script verifies that all required directories and files are properly set up

set -e

echo "🔍 Validating Project Scaffolding..."
echo ""

# Colors for output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

ERRORS=0
WARNINGS=0

# Function to check if a directory exists
check_dir() {
    if [ -d "$1" ]; then
        echo -e "${GREEN}✅${NC} Directory exists: $1"
    else
        echo -e "${RED}❌${NC} Directory missing: $1"
        ((ERRORS++))
    fi
}

# Function to check if a file exists
check_file() {
    if [ -f "$1" ]; then
        echo -e "${GREEN}✅${NC} File exists: $1"
    else
        echo -e "${RED}❌${NC} File missing: $1"
        ((ERRORS++))
    fi
}

# Function to check if a file is not empty
check_file_not_empty() {
    if [ -f "$1" ] && [ -s "$1" ]; then
        echo -e "${GREEN}✅${NC} File exists and not empty: $1"
    elif [ -f "$1" ]; then
        echo -e "${YELLOW}⚠️${NC}  File exists but is empty: $1"
        ((WARNINGS++))
    else
        echo -e "${RED}❌${NC} File missing: $1"
        ((ERRORS++))
    fi
}

echo "📁 Checking Directory Structure..."
echo ""

# Check main directories
check_dir "src"
check_dir "src/backend"
check_dir "src/frontend"
check_dir "tests"
check_dir "tests/unit"
check_dir "tests/integration"
check_dir "tests/e2e"
check_dir "config"
check_dir "config/dev"
check_dir "config/test"
check_dir "config/prod"
check_dir "infra"
check_dir "docs"
check_dir "docs/architecture"
check_dir "docs/api"
check_dir "docs/guides"

echo ""
echo "📄 Checking Configuration Files..."
echo ""

# Check configuration files
check_file ".env.template"
check_file ".editorconfig"
check_file ".gitignore"
check_file ".gitattributes"
check_file ".pre-commit-config.yaml"
check_file "mkdocs.yml"
check_file "package.json"
check_file "requirements.txt"
check_file "requirements-dev.txt"

echo ""
echo "📝 Checking Documentation Files..."
echo ""

# Check documentation files
check_file "README.md"
check_file "CHANGELOG.md"
check_file "CONTRIBUTING.md"
check_file "LICENSE.md"
check_file_not_empty "docs/index.md"
check_file_not_empty "docs/README.md"
check_file_not_empty "docs/architecture/README.md"
check_file_not_empty "docs/api/README.md"
check_file_not_empty "docs/guides/README.md"

echo ""
echo "📋 Checking README Files in Directories..."
echo ""

# Check README files in key directories
check_file_not_empty "src/backend/README.md"
check_file_not_empty "src/frontend/README.md"
check_file_not_empty "tests/README.md"
check_file_not_empty "tests/unit/README.md"
check_file_not_empty "tests/integration/README.md"
check_file_not_empty "tests/e2e/README.md"
check_file_not_empty "config/README.md"
check_file_not_empty "config/dev/README.md"
check_file_not_empty "config/test/README.md"
check_file_not_empty "config/prod/README.md"
check_file_not_empty "infra/README.md"

echo ""
echo "🔧 Checking File Syntax..."
echo ""

# Check JSON syntax
if command -v python3 &> /dev/null; then
    if python3 -c "import json; json.load(open('package.json'))" 2>/dev/null; then
        echo -e "${GREEN}✅${NC} package.json is valid JSON"
    else
        echo -e "${RED}❌${NC} package.json has invalid JSON syntax"
        ((ERRORS++))
    fi
fi

# Check YAML syntax (for .pre-commit-config.yaml)
if command -v python3 &> /dev/null; then
    if python3 -c "import yaml; yaml.safe_load(open('.pre-commit-config.yaml'))" 2>/dev/null; then
        echo -e "${GREEN}✅${NC} .pre-commit-config.yaml is valid YAML"
    else
        echo -e "${RED}❌${NC} .pre-commit-config.yaml has invalid YAML syntax"
        ((ERRORS++))
    fi
fi

echo ""
echo "📊 Validation Summary"
echo "═══════════════════════════════════════════"

if [ $ERRORS -eq 0 ] && [ $WARNINGS -eq 0 ]; then
    echo -e "${GREEN}✅ All checks passed!${NC}"
    echo ""
    echo "Project scaffolding is complete and valid."
    exit 0
elif [ $ERRORS -eq 0 ]; then
    echo -e "${YELLOW}⚠️  Validation completed with $WARNINGS warning(s)${NC}"
    echo ""
    echo "Project scaffolding is mostly complete."
    exit 0
else
    echo -e "${RED}❌ Validation failed with $ERRORS error(s) and $WARNINGS warning(s)${NC}"
    echo ""
    echo "Please fix the errors listed above."
    exit 1
fi

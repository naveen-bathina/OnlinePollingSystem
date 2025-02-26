# Use official Node.js image
FROM node:18

# Set working directory
WORKDIR /app

# Copy package.json and install dependencies
COPY package.json package-lock.json ./
RUN npm install

# Copy the rest of the app
COPY . .

# Expose the port for Vite development server
EXPOSE 5173

# Start the React app using Vite
CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0", "--port", "5173"]

import axios from 'axios';

export default class DeviceApiService {
    private baseUrl: string;

    constructor(baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    async getDeviceId(): Promise<DeviceDetails> {
        const response = await axios.get(`${this.baseUrl}/device/id`);
        return response.data;
    }
}

export interface DeviceDetails {
    deviceId: string;
}
import axios from 'axios';
import { Constants } from '../constants';

export default class DeviceApiService {
    private baseUrl: string;

    constructor() {
        this.baseUrl = Constants.API_BASE_URL;
    }

    async getDeviceId(): Promise<DeviceDetails> {
        const response = await axios.get(`${this.baseUrl}/device/id`);
        return response.data;
    }
}

export interface DeviceDetails {
    deviceId: string;
}
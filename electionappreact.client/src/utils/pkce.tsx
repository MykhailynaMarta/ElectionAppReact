// src/utils/pkce.ts
// простий PKCE генератор: створює code_verifier та code_challenge (S256)

function base64UrlEncode(buffer: ArrayBuffer) {
    // base64 без +/=
    const bytes = new Uint8Array(buffer);
    let str = "";
    for (let i = 0; i < bytes.byteLength; i++) {
        str += String.fromCharCode(bytes[i]);
    }
    const base64 = btoa(str);
    return base64.replace(/\+/g, "-").replace(/\//g, "_").replace(/=+$/, "");
}

export async function generatePkcePair() {
    // генеруємо випадковий verifier
    const array = new Uint8Array(64);
    crypto.getRandomValues(array);
    const verifier = base64UrlEncode(array.buffer);

    // SHA-256 of verifier -> challenge
    const encoder = new TextEncoder();
    const data = encoder.encode(verifier);
    const digest = await crypto.subtle.digest("SHA-256", data);
    const challenge = base64UrlEncode(digest);

    return { verifier, challenge };
}
